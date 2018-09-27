namespace Plugin.Common
{
    using App.Common.Hash;
    using System;
    public interface ITestContext: IHastable
    {
        Authorization Auth { get; }
        EventArgs EventArgs { get;}
        Guid Id { get; }
        void SetEventArgs(EventArgs ev);
        void Add(string key, object value);
        void SetAuthorization(Authorization authorization);
    }
}
