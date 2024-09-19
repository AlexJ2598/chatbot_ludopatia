namespace chatbot_ludo.Web.Data
{
    using chatbot_ludo.Web.Data.Entities;
    public interface IRepository
    {
        void AddConsejo(Consejo consejo);

        IEnumerable<Consejo> GetConsejos();

        Consejo GetConsejo(int id);

        bool ConsejoExists(int id);

        void RemoveConsejo(Consejo consejo);

        Task<bool> SaveAllAsync();

        void UpdateConsejo(Consejo consejo);

    }
}