﻿using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Couchbase.Configuration.Client;
using Couchbase.Extensions.Encryption.Providers;
using Couchbase.Extensions.Encryption.Stores;
using Xunit;

namespace Couchbase.Extensions.Encryption.IntegrationTests
{
    public class FieldEncryptionTests
    {
        [Fact]
        public void Test_Encrypt_String()
        {
            var key = "!mysecretkey#9^5usdk39d&dlf)03sL";

            var config = new ClientConfiguration(TestConfiguration.GetConfiguration());
            config.EnableFieldEncryption(new AesCryptoProvider(new InsecureKeyStore(
                new KeyValuePair<string, string>("publickey", key),
                new KeyValuePair<string, string>("mysecret", "myauthpassword")))
            {
                PublicKeyName = "publickey",
                PrivateKeyName = "mysecret"
            });

            using (var cluster = new Cluster(config))
            {
                cluster.Authenticate("Administrator", "password");
                var bucket = cluster.OpenBucket();
                var poco = new Poco
                {
                    Bar = "Bar",
                    Foo = 90,
                    ChildObject = new PocoMoco
                    {
                        Bar = "Bar2"
                    },
                    Fizz = "fizz",
                    Baz = new List<int> {3, 4}
                };

                var result = bucket.Upsert("thepoco", poco);

                Assert.True(result.Success);

                var get = bucket.Get<Poco>("thepoco");
                Assert.True(get.Success);
                Assert.Equal("Bar", get.Value.Bar);
                Assert.Equal(90, get.Value.Foo);
                Assert.Equal(new List<int> {3, 4}, get.Value.Baz);
                Assert.Equal("Bar2", get.Value.ChildObject.Bar);
            }
        }

        [Fact]
        public void Test_Encrypt2_String()
        {
                var key = "!mysecretkey#9^5usdk39d&dlf)03sL";

                var config = new ClientConfiguration(TestConfiguration.GetConfiguration());
            config.EnableFieldEncryption(new AesCryptoProvider(new InsecureKeyStore(
                new KeyValuePair<string, string>("mypublickey", key),
                new KeyValuePair<string, string>("myauthsecret", "myauthpassword")))
            {
                PublicKeyName = "mypublickey",
                PrivateKeyName = "myauthsecret"
            });

                using (var cluster = new Cluster(config))
                {
                    cluster.Authenticate("Administrator", "password");
                    var bucket = cluster.OpenBucket();

                    var poco = new Poco2
                    {
                        Message = "The old grey goose jumped over the wrickety gate."
                    };
                    var result = bucket.Upsert("thepoco2_", poco);

                    Assert.True(result.Success);

                    var get = bucket.Get<Poco2>("thepoco2");
                    Assert.True(get.Success);
            }
        }

        [Fact]
        public void Test_Encrypt2_Int()
        {
            var key = "!mysecretkey#9^5usdk39d&dlf)03sL";

            var config = new ClientConfiguration(TestConfiguration.GetConfiguration());
            config.EnableFieldEncryption(new AesCryptoProvider(new InsecureKeyStore(
                new KeyValuePair<string, string>("mypublickey", key),
                new KeyValuePair<string, string>("myauthsecret", "myauthpassword")))
            {
                PublicKeyName = "mypublickey",
                PrivateKeyName = "myauthsecret"
            });

            using (var cluster = new Cluster(config))
            {
                cluster.Authenticate("Administrator", "password");
                var bucket = cluster.OpenBucket();

                var poco = new PocoWithInt()
                {
                    Message = 10
                };
                var result = bucket.Upsert("thepoco2_int_", poco);

                Assert.True(result.Success);

                var get = bucket.Get<Poco2>("thepoco2");
                Assert.True(get.Success);
            }
        }

        [Fact]
        public void Test_Encrypt2_IntString()
        {
            var key = "!mysecretkey#9^5usdk39d&dlf)03sL";

            var config = new ClientConfiguration(TestConfiguration.GetConfiguration());
            config.EnableFieldEncryption(new AesCryptoProvider(new InsecureKeyStore(
                new KeyValuePair<string, string>("mypublickey", key),
                new KeyValuePair<string, string>("myauthsecret", "myauthpassword")))
            {
                PublicKeyName = "mypublickey",
                PrivateKeyName = "myauthsecret"
            });

            using (var cluster = new Cluster(config))
            {
                cluster.Authenticate("Administrator", "password");
                var bucket = cluster.OpenBucket();

                var poco = new PocoWithString()
                {
                    Message = "10"
                };
                var result = bucket.Upsert("thepoco2_intstring", poco);

                Assert.True(result.Success);

                var get = bucket.Get<Poco2>("thepoco2");
                Assert.True(get.Success);
            }
        }

        [Fact]
        public void Test_Encrypt_Array()
        {
            var key = "!mysecretkey#9^5usdk39d&dlf)03sL";

            var config = new ClientConfiguration(TestConfiguration.GetConfiguration());
            config.EnableFieldEncryption(new AesCryptoProvider(new InsecureKeyStore(
                new KeyValuePair<string, string>("mypublickey", key),
                new KeyValuePair<string, string>("myauthsecret", "myauthpassword")))
            {
                PublicKeyName = "mypublickey",
                PrivateKeyName = "myauthsecret"
            });

            using (var cluster = new Cluster(config))
            {
                cluster.Authenticate("Administrator", "password");
                var bucket = cluster.OpenBucket();

                var poco = new PocoWithArray()
                {
                    Message = new List<string>
                    {
                        "The",
                        "Old",
                        "Grey",
                        "Goose",
                        "Jumped",
                        "over",
                        "the",
                        "wrickety",
                        "gate"
                    }
                };
                var result = bucket.Upsert("pocowitharray_", poco);

                Assert.True(result.Success);

                var get = bucket.Get<PocoWithArray>("pocowitharray");
                Assert.True(get.Success);
            }
        }


        [Fact]
        public void Test_Encrypt_NestedObject()
        {
            var key = "!mysecretkey#9^5usdk39d&dlf)03sL";

            var config = new ClientConfiguration(TestConfiguration.GetConfiguration());
            config.EnableFieldEncryption(new AesCryptoProvider(new InsecureKeyStore(
                new KeyValuePair<string, string>("mypublickey", key),
                new KeyValuePair<string, string>("myauthsecret", "myauthpassword")))
            {
                PublicKeyName = "mypublickey",
                PrivateKeyName = "myauthsecret"
            });

            using (var cluster = new Cluster(config))
            {
                cluster.Authenticate("Administrator", "password");
                var bucket = cluster.OpenBucket();

                var poco = new PocoWithObject
                {
                    Message = new InnerObject
                    {
                        MyInt = 10,
                        MyValue = "The old grey goose jumped over the wrickety gate."
                    }
                };
                var result = bucket.Upsert("mypocokey", poco);

                var get = bucket.Get<PocoWithObject>("mypocokey");
            }
        }

        public class PocoWithObject
        {
            [EncryptedField(Provider = "AES-256-HMAC-SHA256")]
            public InnerObject Message { get; set; }
        }

        public class InnerObject
        {
            public string MyValue { get; set; }

            public int MyInt { get; set; }
        }


        public class PocoWithInt
        {
            [EncryptedField(Provider = "AES-256-HMAC-SHA256")]
            public int Message { get; set; }
        }

        public class PocoWithString
        {
            [EncryptedField(Provider = "AES-256-HMAC-SHA256")]
            public string Message { get; set; }
        }


        public class Poco2
        {
            [EncryptedField(Provider = "AES-256-HMAC-SHA256")]
            public string Message { get; set; }
        }

        public class PocoWithArray
        {
            [EncryptedField(Provider = "AES-256-HMAC-SHA256")]
            public List<string> Message { get; set; }
        }


        public class Poco
        {
            [EncryptedField(Provider = "AES-256-HMAC-SHA256")]
            public string Bar { get; set; }

            [EncryptedField(Provider = "AES-256-HMAC-SHA256")]
            public int Foo { get; set; }

            [EncryptedField(Provider = "AES-256-HMAC-SHA256")]
            public List<int> Baz { get; set; }

            [EncryptedField(Provider = "AES-256-HMAC-SHA256")]
            public PocoMoco ChildObject { get; set; }

            public string Fizz { get; set; }
        }

        public class PocoMoco
        {
            public string Bar { get; set; }
        }
    }
}
