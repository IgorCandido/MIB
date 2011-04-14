// Type: Rhino.Mocks.Expect
// Assembly: Rhino.Mocks, Version=3.6.0.0, Culture=neutral, PublicKeyToken=0b3305902db7183f
// Assembly location: C:\Igor\Isel\Semestres\Projecto Mestrado\MIB\Source\MIB\Libraries\RhinoMocks\Rhino.Mocks.dll

using Rhino.Mocks.Interfaces;

namespace Rhino.Mocks
{
    public static class Expect
    {
        #region Delegates

        public delegate void Action();

        #endregion

        public static IMethodOptions<T> Call<T>(T ignored);
        public static IMethodOptions<Expect.Action> Call(Expect.Action actionToExecute);
        public static ICreateMethodExpectation On(object mockedInstace);
    }
}
