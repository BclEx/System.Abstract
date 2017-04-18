#region Foreign-License
// .Net40 Kludge
#endregion
#if NET35

namespace System.Threading
{
    /// <summary>
    /// LazyThreadSafetyMode
    /// </summary>
	public enum LazyThreadSafetyMode
	{
        /// <summary>
        /// 
        /// </summary>
		None,
        /// <summary>
        /// 
        /// </summary>
		PublicationOnly,
        /// <summary>
        /// 
        /// </summary>
		ExecutionAndPublication
	}
}
#endif