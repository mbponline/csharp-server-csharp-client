namespace Client.Modules.Utils.DAL.Common
{

    public class ServiceOperation<TFunction, TAction>
    {
        public TFunction Function { get; set; }

        public TAction Action { get; set; }
    }

}