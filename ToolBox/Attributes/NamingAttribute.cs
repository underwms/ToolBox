using System;

namespace ToolBox
{
    /// <summary>
    /// Class of custom attributes that defines naming related attributes for enumerated fields.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class NamingAttribute : System.Attribute
    {

        #region Constructors

        /// <summary>
        /// Creates a new instance of NamingAttribute.
        /// </summary>
        public NamingAttribute()
        {
        }

        /// <summary>
        /// Creates a new <see cref="NamingAttribute"/> instance with the specified friendly name.
        /// </summary>
        /// <param name="friendlyName">The friendly name for the value.</param>
        public NamingAttribute(String friendlyName)
            : this(friendlyName, null)
        {
        }

        /// <summary>
        /// Creates a new <see cref="NamingAttribute"/> instance with the specified friendly name
        /// and abbreviation.
        /// </summary>
        /// <param name="friendlyName">The friendly name for the value.</param>
        /// <param name="abbreviation">The abbreviation for the value.</param>
        public NamingAttribute(String friendlyName, String abbreviation)
            : this(friendlyName, abbreviation, null)
        {
        }

        /// <summary>
        /// Creates a new <see cref="NamingAttribute"/> instance with the specified friendly name, 
        /// abbreviation and tool tip.
        /// </summary>
        /// <param name="friendlyName">The friendly name for the value.</param>
        /// <param name="abbreviation">The abbreviation for the value.</param>
        /// <param name="toolTip">The tool tip for the value.</param>
        public NamingAttribute(String friendlyName, String abbreviation, String toolTip)
        {
            Abbreviation = abbreviation;
            FriendlyName = friendlyName;
            ToolTip = toolTip;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// The abbreviated name for the field.
        /// </summary>
        public String Abbreviation
        {
            get;
            set;
        }

        /// <summary>
        /// The friendly name for the field.
        /// </summary>
        public String FriendlyName
        {
            get;
            set;
        }

        /// <summary>
        /// The tool tip value for the field.
        /// </summary>
        public String ToolTip
        {
            get;
            set;
        }

        #endregion Properties
    }
}
