namespace Meeg.Configuration
{
    /// <summary>
    /// Options class used by the <see cref="AppConfigurationBinder"/>.
    /// </summary>
    public class AppConfigurationBinderOptions
    {
        /// <summary>
        /// When false (the default), the binder will only attempt to set public properties.
        /// If true, the binder will attempt to set all non read-only properties.
        /// </summary>
        public bool BindNonPublicProperties { get; set; }
    }
}
