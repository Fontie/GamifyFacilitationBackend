mergeInto(LibraryManager.library, {
    GetDataForUnity: function() {
        if (typeof window.GetDataForUnity === "function") {
            console.log("Calling GetDataForUnity from Unity...");
            window.GetDataForUnity(); // Call the frontend function
        } else {
            console.error("Error: GetDataForUnity is not defined on window.");
        }
    }
});
