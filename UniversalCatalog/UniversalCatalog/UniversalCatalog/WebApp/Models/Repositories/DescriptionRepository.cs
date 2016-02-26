﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApp.Models.Entities;
using WebApp.Models.Repositories.Interfaces;

namespace WebApp.Models.Repositories
{
    public class DescriptionRepository : Repository<Description>, IDescriptionRepository
    {
        public DescriptionRepository(ApplicationDbContext context) : base(context)
        {
        }

        public ApplicationDbContext UniversalCatalogContext
        {
            get { return Context as ApplicationDbContext; }
        }
    }
}