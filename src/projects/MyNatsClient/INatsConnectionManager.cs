using System;
using System.Collections.Generic;
using System.Threading;

namespace MyNatsClient
{
    public interface INatsConnectionManager
    {
        ISocketFactory SocketFactory { set; }

        /// <summary>
        /// Tries to establish a connection to any of the specified hosts in the
        /// sent <see cref="ConnectionInfo"/>.
        /// </summary>
        /// <param name="connectionInfo"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Connection and any received <see cref="IOp"/> during the connection phase.</returns>
        Tuple<INatsConnection, IList<IOp>> OpenConnection(ConnectionInfo connectionInfo, CancellationToken cancellationToken);
    }
}