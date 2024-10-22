namespace sim2kid.Package.SerializedDictionary.Runtime.Parsers
{
    public interface IParser
    {
        /// <summary>
        /// Converts from a string to an object using any method.
        /// </summary>
        /// <param name="value">The string version of the object.</param>
        /// <returns>Returns the object value.</returns>
        public object StringToObject(string value);
        
        /// <summary>
        /// Convert from an object to a string using any method.
        /// </summary>
        /// <param name="value">The object to turn into a string.</param>
        /// <returns>The string version of the object.</returns>
        public string ObjectToString(object value);
    }
}
