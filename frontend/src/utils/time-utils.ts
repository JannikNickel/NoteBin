export const formatDateYYYYMMDD = (utcMilliseconds: number): string => {
    const date = new Date(utcMilliseconds);
    return date.toISOString().split("T")[0];
};

export const formatTimeAgo = (utcMilliseconds: number): string => {
    const now = Date.now();
    const diff = now - utcMilliseconds;

    if (diff < 1000) {
        return "just now";
    }

    const intervals = [
        { label: "y", seconds: 365 * 24 * 60 * 60 },
        { label: "d", seconds: 24 * 60 * 60 },
        { label: "h", seconds: 60 * 60 },
        { label: "m", seconds: 60 },
        { label: "s", seconds: 1 }
    ];

    for (const interval of intervals) {
        const count = Math.floor(diff / (interval.seconds * 1000));
        if (count >= 1) {
            return `${count}${interval.label} ago`;
        }
    }
    return "";
};
