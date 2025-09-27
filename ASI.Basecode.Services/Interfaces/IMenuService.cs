using ASI.Basecode.Data.Models;
using ASI.Basecode.Services.ServiceModels;
using System.Collections.Generic;
using static ASI.Basecode.Resources.Constants.Enums;

namespace ASI.Basecode.Services.Interfaces
{
    public interface IMenuService
    {
        public IEnumerable<MenuReturnViewModel> GetAllMenu();

        public (int statusCode, MenuReturnViewModel createdItem) AddMenu(MenuRequestViewModel inputRequest, int userId);

        public (int statusCode, MenuReturnViewModel updatedItem) UpdateMenu(MenuRequestViewModel inputRequest, int userId);

        public int[] BatchDeleteDocument(IEnumerable<int> inputDocumentIds, int userId);
    }
}
