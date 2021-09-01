using UnityEngine;

public class FirebaseManager : MonoBehaviour
{
    private async void Start()
    {
#if false
        try
        {
            var status = await FirebaseApp.CheckDependenciesAsync();
            if (status != DependencyStatus.Available)
            {
                await FirebaseApp.FixDependenciesAsync().ContinueWith(async t =>
                {
                    status = await FirebaseApp.CheckDependenciesAsync();
                    if (status != DependencyStatus.Available)
                        Debug.LogError($"Couldn't Resolve dependencies error: {status}");
                });
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
#endif
    }
}
