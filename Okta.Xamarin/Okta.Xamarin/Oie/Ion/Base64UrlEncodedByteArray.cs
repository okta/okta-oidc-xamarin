namespace Okta.Xamarin.Oie
{
    /// <summary>
    /// A base 64 encoded byte array.
    /// </summary>
    public class Base64UrlEncodedByteArray
    {
        private byte[] array;
        private string value;

        /// <summary>
        /// Initializes a new instance of the <see cref="Base64UrlEncodedByteArray"/> class.
        /// </summary>
        public Base64UrlEncodedByteArray() { }

        /// <summary>
        /// Gets or sets the base 64 encoded value.
        /// </summary>
        public string Value
        {
            get
            {
                return this.value;
            }

            set
            {
                this.value = value;
                this.array = value.FromBase64UrlEncoded();
            }
        }

        /// <summary>
        /// Gets or sets the byte array.
        /// </summary>
        public byte[] Array
        {
            get
            {
                return this.array;
            }

            set
            {
                this.array = value;
                this.value = value.ToBase64UrlEncoded();
            }
        }
    }
}
