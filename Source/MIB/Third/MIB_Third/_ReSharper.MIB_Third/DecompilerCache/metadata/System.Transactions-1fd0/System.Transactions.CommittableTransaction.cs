// Type: System.Transactions.CommittableTransaction
// Assembly: System.Transactions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// Assembly location: C:\Program Files\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0\Profile\Client\System.Transactions.dll

using System;
using System.Threading;

namespace System.Transactions
{
    [Serializable]
    public sealed class CommittableTransaction : Transaction, IAsyncResult
    {
        public CommittableTransaction();
        public CommittableTransaction(TimeSpan timeout);
        public CommittableTransaction(TransactionOptions options);

        #region IAsyncResult Members

        object IAsyncResult.AsyncState { get; }
        bool IAsyncResult.CompletedSynchronously { get; }
        WaitHandle IAsyncResult.AsyncWaitHandle { get; }
        bool IAsyncResult.IsCompleted { get; }

        #endregion

        public IAsyncResult BeginCommit(AsyncCallback asyncCallback, object asyncState);
        public void Commit();
        public void EndCommit(IAsyncResult asyncResult);
    }
}
