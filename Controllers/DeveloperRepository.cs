using MinimalApi.Models;

namespace MinimalApi.Controllers
{
    public class DeveloperRepository
    {
        private readonly Dictionary<Guid, Developer> _developers = new();

        public void Create(Developer developer)
        {
            if (developer == null) return;

            _developers[developer.Id] = developer;
        }

        public List<Developer> GetAll()
        {
            return _developers.Values.ToList();
        }

        public Developer GetDeveloper(Guid id)
        {
            return _developers[id];
        }

        public void Delete(Guid id)
        {
            _developers.Remove(id);
        }

        public void Update(Developer developer)
        {
            if (GetDeveloper(developer.Id) is null) return;
            _developers[developer.Id] = developer;
        }
    }
}
