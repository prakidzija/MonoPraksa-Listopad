export async function apiRequest(endpoint, options = {}) {
    const token = sessionStorage.getItem("token");

    const response = await fetch(endpoint, {
        ...options, headers: {
            ...options.headers,
            ...(options.body ? { "Content-Type": "application/json" } : {}),
            ...(token ? { Authorization: `Bearer ${token}` } : {})
        }
    });

    const text = await response.text();

    let data = null;

    if (text) {
        try {
            data = JSON.parse(text);
        }
        catch {
            data = text;
        }
    }

    if (!response.ok) {
        const message = typeof data === "string"
            ? data
            : data?.title || `HTTP error: ${response.status}`;

        throw new Error(message);
    }

    return data
}