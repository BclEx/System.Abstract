#region Foreign-License
// .Net40 Polyfill
#endregion
#if !CLR4
using System.Security;
using System.Runtime.InteropServices;

namespace System.Runtime.CompilerServices
{
    [StructLayout(LayoutKind.Sequential), ComVisible(false)]
    internal struct DependentHandle
    {
        private WeakReference _primary;
        private object _secondary;

        [SecurityCritical]
        public DependentHandle(object primary, object secondary)
        {
            _primary = new WeakReference(primary);
            _secondary = secondary;
        }

        public bool IsAllocated
        {
            get { return ((_primary != null) && _primary.IsAlive); }
        }

        [SecurityCritical]
        public object GetPrimary() { return _primary.Target; }

        [SecurityCritical]
        public void GetPrimaryAndSecondary(out object primary, out object secondary)
        {
            primary = _primary.Target;
            secondary = _secondary;
        }

        // Forces dependentHandle back to non-allocated state (if not already there) and frees the handle if needed.
        [SecurityCritical]
        public void Free()
        {
            _primary = null;
            _secondary = null;
        }
    }
}

#endif