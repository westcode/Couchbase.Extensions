﻿using Couchbase.Configuration.Client;
using Couchbase.Extensions.Encryption.Providers;
using Couchbase.Extensions.Encryption.Stores;
using Xunit;

namespace Couchbase.Extensions.Encryption.UnitTests
{
    public class ClientConfigurationTests
    {
        [Fact]
        public void Test_EnableFieldEncryption()
        {
            var config = new ClientConfiguration();
            config.EnableFieldEncryption(new AesCryptoProvider(new InsecureKeyStore("thekeyname", "thekey")));

            Assert.IsType<EncryptedFieldSerializer>(config.Serializer());
        }
    }
}

#region [ License information          ]
/* ************************************************************
 *
 *    @author Couchbase <info@couchbase.com>
 *    @copyright 2017 Couchbase, Inc.
 *
 *    Licensed under the Apache License, Version 2.0 (the "License");
 *    you may not use this file except in compliance with the License.
 *    You may obtain a copy of the License at
 *
 *        http://www.apache.org/licenses/LICENSE-2.0
 *
 *    Unless required by applicable law or agreed to in writing, software
 *    distributed under the License is distributed on an "AS IS" BASIS,
 *    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *    See the License for the specific language governing permissions and
 *    limitations under the License.
 *
 * ************************************************************/
#endregion
