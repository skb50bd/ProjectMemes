using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace XMemes.Services
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// Copies values to a destination object from a source object.
        /// Keeps existing destination values if source doesn't have a property with same name.
        /// </summary>
        /// <typeparam name="TSource">Type of the object to get values from</typeparam>
        /// <typeparam name="TDest">Type of the object to set values to</typeparam>
        /// <param name="dest">Object to set values to</param>
        /// <param name="src">Object to get values from</param>
        /// <returns>Completely new object with values from source.</returns>
        public static TDest? WithValuesFrom<TDest, TSource>(
            this TDest? dest,
            TSource? src) where TSource: class where TDest: class
        {
            var srcProps = src?.GetType().GetProperties();
            if (srcProps is null) return dest;

            var output = dest.Clone();
            foreach (var prop in srcProps)
            {
                var hasWritableProp =
                    dest?.GetType()
                        .GetProperty(prop.Name)
                        ?.CanWrite
                    ?? false;

                if (hasWritableProp)
                    prop.SetValue(output, prop.GetValue(src));
            }

            return output;
        }

        /// <summary>
        /// Perform a deep Copy of the object.
        /// </summary>
        /// <typeparam name="T">The type of object being copied.</typeparam>
        /// <param name="source">The object instance to copy.</param>
        /// <returns>The copied object.</returns>
        public static T? Clone<T>(this T? source) where T: class
        {
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable.", nameof(source));
            }

            // Don't serialize a null object, simply return the default for that object
            if (source is null)
            {
                return null;
            }

            IFormatter formatter = new BinaryFormatter();
            using var stream = new MemoryStream() as Stream;
            formatter.Serialize(stream, source);
            stream.Seek(0, SeekOrigin.Begin);
            return (T)formatter.Deserialize(stream);
        }
    }
}