﻿using System;

using KenticoCloud.ContentManagement.Models.Identifiers;

namespace KenticoCloud.ContentManagement.Models.Items
{
    public sealed class ContentTypeIdentifier : Identifier<ContentTypeIdentifier>
    {
        [Obsolete("For internal purposes. Use static method ContentTypeIdentifier.ByXYZ instead.")]
        public ContentTypeIdentifier() { }
    }
}
