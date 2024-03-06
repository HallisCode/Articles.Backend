using JWT.Algorithms;
using JWT.Serializers;
using JWT;
using Newtonsoft.Json.Linq;
using System;
using Newtonsoft.Json;

namespace Application.Utils
{
	public class JWTEncoder
	{

		public static IJwtEncoder Create()
		{
			IJwtAlgorithm algorithm = new HMACSHA256Algorithm();

			IJsonSerializer serializer = new JsonNetSerializer();

			IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();


			IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

			return encoder;
		}
	}

	public class JWTDecoder
	{

		public static IJwtDecoder Create()
		{
			IJwtAlgorithm algorithm = new HMACSHA256Algorithm();

			IJsonSerializer serializer = new JsonNetSerializer();

			IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();


			IDateTimeProvider provider = new UtcDateTimeProvider();

			IJwtValidator validator = new JwtValidator(serializer, provider);


			IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder, algorithm);

			return decoder;
		}
	}
}
