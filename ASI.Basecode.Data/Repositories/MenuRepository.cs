using ASI.Basecode.Data.EFCore;
using ASI.Basecode.Data.Interfaces;
using ASI.Basecode.Data.Models;
using Basecode.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;

namespace ASI.Basecode.Data.Repositories
{
    public class MenuRepository : BaseRepository<Menu>, IMenuRepository
    {
        public MenuRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public IEnumerable<Menu> GetAllMenu()
        {
            // Use the BaseRepository methods, not direct _context
            return this.GetDbSet<Menu>().Where(x => !x.IsDeleted).ToList();
        }

        public IEnumerable<Menu> GetRecordsByIds(IEnumerable<int> ids)
        {
            return this.GetDbSet<Menu>().Where(x => ids.Contains(x.MenuID)).ToList();
        }

        public Menu GetMenuById(int id)
        {
            return this.GetDbSet<Menu>().FirstOrDefault(x => x.MenuID == id && !x.IsDeleted);
        }

        public void AddMenu(Menu entity)
        {
            using (var transaction = UnitOfWork.CreateTransaction())
            {
                try
                {
                    this.GetDbSet<Menu>().Add(entity);
                    UnitOfWork.SaveChanges();
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public void UpdateMenu(Menu entity)
        {
            using (var transaction = UnitOfWork.CreateTransaction())
            {
                try
                {
                    this.GetDbSet<Menu>().Update(entity);
                    UnitOfWork.SaveChanges();
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        /// <summary>
        /// Deletes a Menu record.
        /// </summary>
        /// <param name="entity">The Menu entity to be deleted.</param>
        public void DeleteDocument(Menu entity)
        {
            this.GetDbSet<Menu>().Update(entity);
            UnitOfWork.SaveChanges();
        }

        public int CheckUniqueMenu(int id, string menuName)
        {
            var menu = this.GetDbSet<Menu>().Any(e => e.MenuID != id &&
                                                     e.MenuName.Equals(menuName) &&
                                                     !e.IsDeleted);
            if (menu)
            {
                return -2;
            }
            return 1;
        }

        public bool CheckDocumentRequestInput(int id)
        {
            var checkExists = this.GetDbSet<Menu>()
                .FirstOrDefault(x => x.MenuID.Equals(id) && !x.IsDeleted);

            if (checkExists != null && id != 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public IEnumerable<int> AddLogList(List<LogMenu> entities)
        {
            using (var transaction = UnitOfWork.CreateTransaction())
            {
                try
                {
                    this.GetDbSet<LogMenu>().AddRange(entities);
                    UnitOfWork.SaveChanges();
                    transaction.Commit();
                    return entities.Select(e => e.LogMenuID).ToArray();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}
