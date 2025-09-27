using ASI.Basecode.Data.Models;
using System.Collections.Generic;

namespace ASI.Basecode.Data.Interfaces
{
    public interface IMenuRepository
    {
        public IEnumerable<Menu> GetAllMenu();

        public IEnumerable<Menu> GetRecordsByIds(IEnumerable<int> ids);

        public Menu GetMenuById(int id);

        public void DeleteDocument(Menu entity);

        public void AddMenu(Menu entity);

        public void UpdateMenu(Menu entity);

        public int CheckUniqueMenu(int id, string menuName);

        public bool CheckDocumentRequestInput(int id);

        public IEnumerable<int> AddLogList(List<LogMenu> entities);
    }
}
