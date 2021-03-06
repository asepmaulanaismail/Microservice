﻿#region Copyright
// Copyright Hitachi Consulting
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//    http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Xigadee
{
    public static class ApiProviderAsyncQueryHelper
    {
        public static IQueryable<E> Query<K, E>(this ApiProviderAsyncV2<K, E> provider) where K: IEquatable<K>
        {
            throw new NotImplementedException();
        }
    }

    //public class ContactEntityTypeSerializer: ODataEntityTypeSerializer
    //{
    //}

    //internal class MyLinq<K, E>: IOrderedQueryable<E>
    //    where K : IEquatable<K>
    //{

    //    QueryProvider provider;

    //    Expression expression;


    //    public Type ElementType
    //    {
    //        get
    //        {
    //            throw new NotImplementedException();
    //        }
    //    }

    //    public Expression Expression
    //    {
    //        get
    //        {
    //            throw new NotImplementedException();
    //        }
    //    }

    //    public IQueryProvider Provider
    //    {
    //        get
    //        {
    //            throw new NotImplementedException();
    //        }
    //    }

    //    public IEnumerator<E> GetEnumerator()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    IEnumerator IEnumerable.GetEnumerator()
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}
