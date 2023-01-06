using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Events.CatalogItem;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistance
{
    public class CatalogItemAggregateRepository_old : ICatalogItemAggregateRepository_old
    {
        private readonly ApplicationDbContext _context;
        public CatalogItemAggregateRepository_old(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }

        public async Task SaveAsync(object @event)
        {
            switch (@event)
            {
                case CatalogItemCreated x: await OnCreated(x); break;
                case CatalogItemUpdated x: await OnUpdated(x); break;
                case CatalogItemDeleted x: await OnDeleted(x); break;
            }

            //throw new NotImplementedException();
        }

        private async Task OnDeleted(CatalogItemDeleted catalogItemDeleted)
        {
            try
            {
                var catalogItem = await _context.CatalogItems.AsNoTracking().FirstOrDefaultAsync(x => x.Id == catalogItemDeleted.AggregateId);

                if(catalogItem is null)
                {
                    throw new Exception("Catalog item not found");
                }

                var updatedCatalogItem = new CatalogItem(catalogItem.Id, catalogItem.Name, catalogItem.Description, catalogItem.Price, catalogItem.AvailableStock,
                    catalogItem.RestockThreshold, catalogItem.MaxStockThreshold, catalogItem.OnReorder);

                updatedCatalogItem.ClearEvents();

                _context.Entry(updatedCatalogItem).Property(x => x.Id).IsModified = false;
                _context.Entry(updatedCatalogItem).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task OnUpdated(CatalogItemUpdated catalogItemUpdated)
        {
            try
            {
                var catalogItem = await _context.CatalogItems.AsNoTracking().FirstOrDefaultAsync(x => x.Id == catalogItemUpdated.AggregateId);

                if(catalogItem is null)
                {
                    throw new Exception("Catalog item not found");
                }

                var updatedCatalogItem = new CatalogItem(catalogItem.Id, catalogItem.Name, catalogItem.Description, catalogItem.Price, catalogItem.AvailableStock,
                    catalogItem.RestockThreshold, catalogItem.MaxStockThreshold, catalogItem.OnReorder);

                updatedCatalogItem.ClearEvents();

                _context.Entry(updatedCatalogItem).Property(x => x.Id).IsModified=false;
                _context.Entry(updatedCatalogItem).State=EntityState.Modified;

                await _context.SaveChangesAsync();

            }
            catch(Exception ex)
            {
                throw;
            }
        }

        private async Task OnCreated(CatalogItemCreated catalogItemCreated)
        {
            try
            {
                var catalogItem = new CatalogItem(catalogItemCreated.AggregateId, catalogItemCreated.Name, catalogItemCreated.Description,
                    catalogItemCreated.Price, catalogItemCreated.AvailableStock, catalogItemCreated.RestockThreshold, catalogItemCreated.MaxStockThreshold, catalogItemCreated.OnReorder);

                catalogItem.ClearEvents();

                await _context.CatalogItems.AddAsync(catalogItem);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
