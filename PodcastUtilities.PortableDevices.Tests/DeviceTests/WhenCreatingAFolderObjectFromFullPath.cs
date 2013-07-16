﻿#region License
// FreeBSD License
// Copyright (c) 2010 - 2013, Andrew Trevarrow and Derek Wilson
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
// 
// Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
// 
// Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED 
// WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A 
// PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR
// ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED
// TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)
// HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING 
// NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
// POSSIBILITY OF SUCH DAMAGE.
#endregion
using NUnit.Framework;
using PortableDeviceApiLib;
using Rhino.Mocks;

namespace PodcastUtilities.PortableDevices.Tests.DeviceTests
{
    public class WhenCreatingAFolderObjectFromFullPath : WhenTestingDevice
    {
        private IDeviceObject DeviceObject { get; set; }

        protected override void GivenThat()
        {
            base.GivenThat();

            PortableDeviceHelper.Stub(
                helper => helper.GetChildObjectIds(PortableDeviceContent, PortableDeviceConstants.WPD_DEVICE_OBJECT_ID))
                .Return(new[] {"Dummy1", "InternalStorageID", "Dummy2"});

            PortableDeviceHelper.Stub(
                helper => helper.GetChildObjectIds(PortableDeviceContent, "InternalStorageID"))
                .Return(new[] {"Dummy3", "Dummy4", "fooId"});

            PortableDeviceHelper.Stub(
                helper => helper.GetChildObjectIds(PortableDeviceContent, "fooId"))
                .Return(new[] {"Dummy5", "Dummy6", "barId"});

            PortableDeviceHelper
                .Stub(propertyHelper => propertyHelper.GetObjectFileName(
                    PortableDeviceContent,
                    "InternalStorageID"))
                .Return("Internal Storage");

            PortableDeviceHelper
                .Stub(propertyHelper => propertyHelper.GetObjectFileName(
                    PortableDeviceContent,
                    "fooId"))
                .Return("foo");

            PortableDeviceHelper
                .Stub(propertyHelper => propertyHelper.GetObjectFileName(
                    PortableDeviceContent,
                    "barId"))
                .Return("bar");
        }

        protected override void When()
        {
            Device.CreateFolderObjectFromPath(@"Internal Storage\Foo\NewFolder");
        }

        [Test]
        public void ItShouldCreateTheCorrectFolder()
        {
            PortableDeviceHelper.AssertWasCalled(
                helper => helper.CreateFolderObject(
                                    Arg<IPortableDeviceContent>.Is.Equal(PortableDeviceContent), 
                                    Arg<string>.Is.Equal("fooId"), 
                                    Arg<string>.Is.Anything));
        }
    }
}