using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using ParcelDeliveryTrackingAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RESTApi.NunitTests
{
    public static class InMemoryContext
    {

        public static ParcelDeliveryTrackingDBContext GeneratedParcels()
        {
            var _contextOptions = new DbContextOptionsBuilder<ParcelDeliveryTrackingDBContext>()
                .UseInMemoryDatabase("ParcelControllerTest")
                .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            return new ParcelDeliveryTrackingDBContext(_contextOptions);
        }

    }
}
