<!-- default badges list -->
![](https://img.shields.io/endpoint?url=https://codecentral.devexpress.com/api/v1/VersionRange/134575677/12.1.7%2B)
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/E4228)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
<!-- default badges end -->
# Initiate a delayed long-running operation and process it on the server without blocking the UI


<p>To implement long-running operations, you can use the following approach:</p><p>- Add an extra property to your object that will show the object's state (processing or processed). This property can be used to modify the view appearance (e.g., disable editors);</p><p>- Add an extra object into your project. This object will be used to determine if there are objects to be processed;</p><p>- Create a service that will check if there are new objects and process them as required.</p><p>With this approach the UI will not freeze and users will be able to continue working.</p>

<br/>


