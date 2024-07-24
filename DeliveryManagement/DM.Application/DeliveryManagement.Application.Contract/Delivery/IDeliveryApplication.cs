using _01_Framework.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryManagement.Application.Contract.Delivery
{
    public interface IDeliveryApplication
    {
        List<DeliveryViewModel> List();
        OperationResult Create(CreateDelivery command);
        OperationResult Edit(EditDelivery command);

    }
}
