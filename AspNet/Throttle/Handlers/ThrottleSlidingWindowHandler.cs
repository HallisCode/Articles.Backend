using AspNet.Throttle.Options;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace AspNet.Throttle.Handlers
{
	public class ThrottleSlidingWindowHandler : ThrottleHandlerBase<ThrottleSlidingWindowOptions>
	{
		public ThrottleSlidingWindowHandler(IMemoryCache memoryCache) : base(memoryCache)
		{
		}

		public override bool Throttle(string key, IThrottleOptions options)
		{
			VerifyOptionsType(options);

			ThrottleSlidingWindowOptions _options = (ThrottleSlidingWindowOptions)options;


			bool isExistContext = memoryCache.TryGetValue(key, out ThrottleSlidingWindowContext? context);

			if (!isExistContext)
			{
				context = SetContext(key, _options);
			}
			else
			{
				// Смещает во времени существование записи, чтобы она не исчезла раньше времени
				memoryCache.Set(key, context, options.TimeInterval);
			}

			// Если закончились токены, проверяем актуальность возрата токенов и возвращаем их
			if (context.TokensAvailable <= 0)
			{
				ExecuteThrottleRules(key, _options, context);

				if (context.TokensAvailable <= 0) return true;
			}


			int indexSegment = CalculateIndexSegment(CalculateIteration(context, _options), _options);

			context.Segments[indexSegment]--;

			context.TokensAvailable--;

			return false;
		}

		// Возвращает токены, которые должны были вернуться с истечением времени.
		protected override void ExecuteThrottleRules(string key, ThrottleSlidingWindowOptions options, IContext context)
		{
			if (context is not ThrottleSlidingWindowContext)
			{
				throw new Exception($"Контекст типа {context.GetType()} не соотвествует ожидаемому типу {typeof(ThrottleSlidingWindowContext)}");
			}

			ThrottleSlidingWindowContext _context = (ThrottleSlidingWindowContext)context;


			int iteration = CalculateIteration(_context, options);

			// Если мы еще не получали возрат токенов за эту итерацию -> true
			// Если это первые итерации -> false
			bool isPaymendWillMade = iteration > _context.LastIterationOfRevert && iteration > options.SegmentsCount;

			if (!isPaymendWillMade) return;


			int indexSegment = CalculateIndexSegment(iteration, options);

			int skippedSegments = iteration - _context.LastIterationOfRevert;

			int segmentsAwaitRevert = skippedSegments > options.SegmentsCount ? options.SegmentsCount : skippedSegments;

			for (int index = indexSegment; index > indexSegment - segmentsAwaitRevert; index--)
			{
				_context.TokensAvailable += Math.Abs(_context.Segments[index]);

				_context.Segments[index] = 0;
			}

			_context.LastIterationOfRevert = iteration;

			memoryCache.Set(key, _context);
		}

		protected override ThrottleSlidingWindowContext SetContext(string key, ThrottleSlidingWindowOptions options)
		{
			ThrottleSlidingWindowContext context = new ThrottleSlidingWindowContext(options.TokenLimit, options.SegmentsCount);

			memoryCache.Set(key, context, options.TimeInterval);

			return context;
		}

		/// <summary>
		/// Высчитывает текущую итерацию, на основе пройденных интервалов, где интервал - TimeInterval / segments
		/// </summary>
		protected int CalculateIteration(ThrottleSlidingWindowContext context, ThrottleSlidingWindowOptions options)
		{
			int iteration = (int)Math.Ceiling((DateTime.UtcNow - context.CreatedAt) / options.Interval);

			return iteration;
		}

		/// <summary>
		/// Высчитывает текущий индек сегмента окна
		/// </summary>
		protected int CalculateIndexSegment(int iteration, ThrottleSlidingWindowOptions options)
		{
			int indexSegment = (iteration - 1) % options.SegmentsCount;

			return indexSegment;
		}


		protected class ThrottleSlidingWindowContext : LimitingContext
		{
			public DateTime CreatedAt { get; private set; }

			public int LastIterationOfRevert { get; set; }

			public int[] Segments { get; set; }


			public ThrottleSlidingWindowContext(int tokenLimit, int segmentsCount) : base(tokenLimit)
			{
				this.CreatedAt = DateTime.UtcNow;

				this.Segments = new int[segmentsCount];

				this.LastIterationOfRevert = segmentsCount;
			}
		}
	}
}
