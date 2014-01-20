using System.Runtime.Serialization;
using Catel.Data;

namespace MyMediaStuff.Data
{
    /// <summary>
    /// ThemeInfo Data object class which fully supports serialization, property changed notifications,
    /// backwards compatibility and error checking.
    /// </summary>
    public class ThemeInfo : DataObjectBase<ThemeInfo>
    {
        #region Variables
        #endregion

        #region Constructor & destructor
        /// <summary>
        /// Initializes a new object from scratch.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="source">The source.</param>
        public ThemeInfo(string name, string source)
        {
            Name = name;
            Source = source;
        }

        /// <summary>
        /// Initializes a new object based on <see cref="SerializationInfo"/>.
        /// </summary>
        /// <param name="info"><see cref="SerializationInfo"/> that contains the information.</param>
        /// <param name="context"><see cref="StreamingContext"/>.</param>
        protected ThemeInfo(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the name of the theme.
        /// </summary>
        public string Name
        {
            get { return GetValue<string>(NameProperty); }
            set { SetValue(NameProperty, value); }
        }

        /// <summary>
        /// Register the Name property so it is known in the class.
        /// </summary>
        public static readonly PropertyData NameProperty = RegisterProperty("Name", typeof(string), string.Empty);

        /// <summary>
        /// Gets or sets the theme source.
        /// </summary>
        public string Source
        {
            get { return GetValue<string>(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        /// <summary>
        /// Register the Source property so it is known in the class.
        /// </summary>
        public static readonly PropertyData SourceProperty = RegisterProperty("Source", typeof(string), string.Empty);
        #endregion

        #region Methods
        #endregion
    }
}