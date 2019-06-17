using ShareCar.Db.Entities;
using ShareCar.Dto.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
﻿using ShareCar.Dto;

namespace ShareCar.Logic.Address_Logic
{
   public interface IAddressLogic
    {
        void UpdateAddress(AddressDto address);
        void AddNewAddress(AddressDto address);
        AddressDto GetAddressById(int id);
        int GetAddressId(AddressDto address);
    }
}
