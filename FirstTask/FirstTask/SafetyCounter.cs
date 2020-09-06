using System.Threading;

namespace TestTasksBarsGroup.FirstTask
{
    public class SafetyCounter : ISafetyCounter
    {
        private ReaderWriterLockSlim _RWlock = new ReaderWriterLockSlim();
        
        private int _count;
        
        public int GetCount()
        {
            _RWlock.EnterReadLock();
            try
            {
                return _count;
            }
            finally
            {
                _RWlock.ExitReadLock();
            }
        }

        public void AddToCount(int val)
        {
            _RWlock.EnterWriteLock();
            try
            {
                _count += val;
            }
            finally
            {
                _RWlock.ExitWriteLock();    
            }
        }
    }
}