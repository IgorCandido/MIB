// Type: System.Transactions.TransactionScope
// Assembly: System.Transactions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// Assembly location: C:\Program Files\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0\Profile\Client\System.Transactions.dll

using System;

namespace System.Transactions
{
    public sealed class TransactionScope : IDisposable
    {
        public TransactionScope();
        public TransactionScope(TransactionScopeOption scopeOption);
        public TransactionScope(TransactionScopeOption scopeOption, TimeSpan scopeTimeout);
        public TransactionScope(TransactionScopeOption scopeOption, TransactionOptions transactionOptions);

        public TransactionScope(TransactionScopeOption scopeOption, TransactionOptions transactionOptions,
                                EnterpriseServicesInteropOption interopOption);

        public TransactionScope(Transaction transactionToUse);
        public TransactionScope(Transaction transactionToUse, TimeSpan scopeTimeout);

        public TransactionScope(Transaction transactionToUse, TimeSpan scopeTimeout,
                                EnterpriseServicesInteropOption interopOption);

        #region IDisposable Members

        public void Dispose();

        #endregion

        public void Complete();
    }
}
