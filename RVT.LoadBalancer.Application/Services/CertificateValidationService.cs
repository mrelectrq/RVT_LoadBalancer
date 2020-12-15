using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RVT.LoadBalancer.Application.Services
{
    public class CertificateValidationService
    {

        public bool ValidateCertificate(string thumbprint)
        {


            List<string> certDataAcces = new List<string>
            {
                 "CCE345C259D0F370FA14D53CFB932347C3F0C4E9",//Node1
                "6F4DCF62CE043F3E79CD27BEDC7D3B3585C62360", //Node2
                "E0E9777D0CAFC1F8248E310C6F6245619881A6F4", // Node3
                "1AA777C4BB46997EC50BEB8040A330AAD99870AC", // 4
                "627B1A87FA77D497824F1AFE14D134E175D11192",//admin
            };

            if (certDataAcces.Contains(thumbprint))
            {
                return true;
            }
            else return false;

        }
    }
}
