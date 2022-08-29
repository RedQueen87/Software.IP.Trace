using Caliburn.Micro;
using System.Reactive.Subjects;

namespace Software.IP.Trace.Common.Helpers;

public static class PropertyChangedBaseExtensions {
    public static bool Set<T>(this PropertyChangedBase @this, BehaviorSubject<T> subject, T newValue, params string[] propertyNames) {
        var oldValue = subject.Value;
        var isOk = @this.Set(ref oldValue, newValue);
        if (!isOk)
            return false;

        subject.OnNext(oldValue);
        @this.NotifyOfPropertyChange(propertyNames);
        return true;
    }

    public static bool NotifyOfPropertyChange(this PropertyChangedBase @this, params string[] propertyNames) {
        foreach (var propertyName in propertyNames)
            @this.NotifyOfPropertyChange(propertyName);
        return true;
    }
}