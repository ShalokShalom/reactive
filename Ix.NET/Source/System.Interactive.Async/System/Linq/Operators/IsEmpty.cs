﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq
{
    public static partial class AsyncEnumerableEx
    {
        public static Task<bool> IsEmptyAsync<TSource>(this IAsyncEnumerable<TSource> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
                throw Error.ArgumentNull(nameof(source));

            return Core(source, cancellationToken);

            static async Task<bool> Core(IAsyncEnumerable<TSource> _source, CancellationToken _cancellationToken)
            {
                var e = _source.GetAsyncEnumerator(_cancellationToken).ConfigureAwait(false);

                try // REVIEW: Can use `await using` if we get pattern bind (HAS_AWAIT_USING_PATTERN_BIND)
                {
                    return !await e.MoveNextAsync();
                }
                finally
                {
                    await e.DisposeAsync();
                }
            }
        }
    }
}
