using SAPbobsCOM;
using SAPbouiCOM;

namespace Sistema_Produccion_3_Backend.Services.SAP.HANA
{
    public class HANAConnection
    {
        public static SAPbobsCOM.Company? OCompany { get; set; }
        public static int RetVal { get; set; } = -1;

        public static void sapConn()
        {
            // Conexion a HANA -------------------------------------
            OCompany = new SAPbobsCOM.Company();
            OCompany.Server = "TN1@192.168.2.246:30013";
            OCompany.UserName = "manager";
            OCompany.Password = "Consap1.";
            OCompany.LicenseServer = "192.168.2.246";
            OCompany.DbUserName = "SAPDBA";
            OCompany.DbPassword = "IMultiple$1992.";
            OCompany.CompanyDB = "RESPALDO_1";
            OCompany.DbServerType = BoDataServerTypes.dst_HANADB;
            OCompany.UseTrusted = true;
            RetVal = OCompany.Connect();
            
            //------------------------------------------------------
        }
    }
}
