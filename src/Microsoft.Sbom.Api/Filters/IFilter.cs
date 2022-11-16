﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Sbom.Api.Filters
{
    public interface IFilter<T>
        where T : IFilter<T>
    {
        bool IsValid(string filePath);

        void Init();
    }
}
