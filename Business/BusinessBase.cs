using Common;
using DBConnector;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business
{
    public class BusinessBase
    {
        protected DBConnectorFichier ConnectorFichier;
        protected DBConnectorSerie ConnectorSerie;

        public BusinessBase()
        {
            ConnectorSerie = new DBConnectorSerie();
            ConnectorFichier = new DBConnectorFichier();
            MyMapper.Initialize();
            Notifications.Initialize();
        }
    }
}
