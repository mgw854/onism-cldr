using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Onism.Cldr.Extensions;

namespace Onism.Cldr.Tools
{
    internal static class JObjectExtensions
    {
        /// <summary>
        /// Ensures this JObject contains a property of a specific Name and value type.
        /// </summary>
        public static JObject PropertiesShouldContain(this JObject obj, string name, JTokenType valueType)
        {
            var property = obj.Properties().FirstOrDefault(x => x.Name == name);

            if (property == null)
                throw new FormatException($"Required property is missing: {name}.");

            property.Value.TokenTypeShouldBe(valueType);

            return obj;
        }

        /// <summary>
        /// Ensures the actual number of properties in this JObject
        /// is equal to their expected number.
        /// </summary>
        public static JObject PropertiesCountShouldBe(this JObject obj, int expected)
        {
            var actual = obj.Properties().Count();

            if (actual != expected)
                throw new FormatException($"Expected {expected} properties, but found {actual}.");

            return obj;
        }

        public static CldrLocale LocaleCodeShouldBe(this CldrLocale obj, string localeCode)
        {
            if (obj.Code != localeCode)
                throw new FormatException($"Expected {localeCode} but found {obj.Code} code.");

            return obj;
        }

        public static JToken TokenTypeShouldBe(this JToken token, JTokenType type)
        {
            if (token.Type != type)
                throw new FormatException($"Expcted {type} type, but found {token.Type}.");

            return token;
        }

        /// <summary>
        /// JSON files are assumed to consist exclusiely of the specified types.
        /// If any other <see cref="JTokenType"/> is found, a file is considered invalid.
        /// </summary>
        private static HashSet<JTokenType> supportedTokenTypes = new HashSet<JTokenType>
        {
            JTokenType.Object,
            JTokenType.Property,
            JTokenType.String
        };

        /// <summary>
        /// Ensures this JObject consists exclusively of supported types.
        /// </summary>
        public static void CheckSupportedTypes(JObject obj)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            var unsupportedTypes = obj.GetAllTypes().Except(supportedTokenTypes).ToArray();

            if (unsupportedTypes.IsNotEmpty())
                throw new FormatException($"Unsupported JTokenTypes used in JSON: {string.Join(", ", unsupportedTypes)}.");
        }
    }
}