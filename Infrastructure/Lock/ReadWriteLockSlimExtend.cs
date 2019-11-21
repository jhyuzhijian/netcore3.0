using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Infrastructure.Lock
{
    /// <summary>
    /// 使用using释放资源方式简化readwritelock的try finally写法(try{加读写锁;业务;}finally{释放读写锁})
    /// readwritelock不能同时进入读锁状态和写锁状态
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class UsingLock<T> : IDisposable
    {
        private struct Lock : IDisposable
        {
            /// <summary>
            /// 读写锁对象
            /// </summary>
            private ReaderWriterLockSlim _Lock;
            /// <summary>
            /// 是否是写入模式
            /// </summary>
            private bool _IsWrite;
            public Lock(ReaderWriterLockSlim rwl, bool IsWrite)
            {
                _Lock = rwl;
                _IsWrite = IsWrite;
            }
            /// <summary>
            /// 释放对象时推出指定的锁定模式
            /// </summary>
            public void Dispose()
            {
                if (_IsWrite)
                {
                    if (_Lock.IsWriteLockHeld)
                    {
                        _Lock.ExitWriteLock();
                    }
                }
                else
                {
                    if (_Lock.IsReadLockHeld)
                    {
                        _Lock.ExitReadLock();
                    }
                }
            }
        }
        /// <summary>
        /// 空的可释放对象，免去了调用时需要判断是否为null的问题
        /// </summary>
        private class Disposable : IDisposable
        {
            public static readonly Disposable Empty = new Disposable();
            public void Dispose() { }
        }
        /// <summary> 读写锁
        /// </summary>
        private ReaderWriterLockSlim _LockSlim = new ReaderWriterLockSlim();
        /// <summary>
        /// 保存数据
        /// </summary>
        private T _Data;

        public UsingLock()
        {
            Enabled = true;
        }
        public UsingLock(T data)
        {
            Enabled = true;
            _Data = data;
        }
        /// <summary>
        /// 是否启用，当该值为false时,Read()和Write()方法将返回Disposable.Empty
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// 获取或设置当前对象中保存数据的值
        /// </summary>
        public T Data
        {
            get
            {
                if (_LockSlim.IsReadLockHeld || _LockSlim.IsWriteLockHeld)
                {
                    return _Data;
                }
                throw new MemberAccessException("请先进入读取或写入锁定模式再进行操作");
            }
            set
            {
                if (_LockSlim.IsWriteLockHeld == false)
                {
                    throw new MemberAccessException("只有写入模式才能修改Data的值");
                }
                _Data = value;
            }
        }
        public IDisposable Read()
        {
            if (Enabled == false || _LockSlim.IsReadLockHeld || _LockSlim.IsWriteLockHeld)
            {
                return Disposable.Empty;
            }
            else
            {
                _LockSlim.EnterReadLock();
                return new Lock(_LockSlim, false);
            }
        }

        public IDisposable Write()
        {
            if (Enabled == false || _LockSlim.IsWriteLockHeld)
            {
                return Disposable.Empty;
            }
            else if (_LockSlim.IsReadLockHeld)
            {
                throw new NotImplementedException("读取模式下无法进入写入锁定状态");
            }
            else
            {
                _LockSlim.EnterWriteLock();
                return new Lock(_LockSlim, true);
            }
        }

        public void Dispose()
        {
            _LockSlim.Dispose();
        }
    }
}
