# Initiate a delayed long-running operation and process it on the server without blocking the UI


<p>To implement long-running operations, you can use the following approach:</p><p>- Add an extra property to your object that will show the object's state (processing or processed). This property can be used to modify the view appearance (e.g., disable editors);</p><p>- Add an extra object into your project. This object will be used to determine if there are objects to be processed;</p><p>- Create a service that will check if there are new objects and process them as required.</p><p>With this approach the UI will not freeze and users will be able to continue working.</p>

<br/>


