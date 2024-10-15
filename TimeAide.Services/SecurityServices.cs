using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using TimeAide.Models.Models;
using TimeAide.Common.Helpers;

namespace TimeAide.Services
{
    public class SecurityServices
    {

        //public void verifyDevice( userid, HttpRequestBase request)
        //{

        //    String ip = SecurityAlert.ExtractIp(request);
        //    String location = SecurityAlert.GetIpLocation(ip);

        //    String deviceDetails = SecurityAlert.GetDeviceDetails(request);

        //    DeviceMetaData existingDevice
        //      = findExistingDevice(userid, deviceDetails, location);

        //    if (existingDevice==null)
        //    {
        //        unknownDeviceNotification(deviceDetails, location,
        //          ip, user.getEmail(), request.getLocale());

        //        DeviceMetaData deviceMetadata = new DeviceMetaData();
             
        //    }
        //    else
        //    {
        //        existingDevice.setLastLoggedIn(new Date());
        //        deviceMetadataRepository.save(existingDevice);
        //    }
        //}


        //private DeviceMetadata findExistingDevice(int userId, String deviceDetails, String location)
        //{
        //    List<DeviceMetadata> knownDevices
        //      = deviceMetadataRepository.findByUserId(userId);

        //    for (DeviceMetadata existingDevice : knownDevices)
        //    {
        //        if (existingDevice.getDeviceDetails().equals(deviceDetails)
        //          && existingDevice.getLocation().equals(location))
        //        {
        //            return existingDevice;
        //        }
        //    }
        //    return null;
        //}

    }
}
