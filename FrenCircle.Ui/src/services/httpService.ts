import { showToast } from "../lib/toastUtils";

const BASE_URL = process.env.NEXT_PUBLIC_API_BASE_URL;

const request = async (
  method: "GET" | "POST",
  url: string,
  data?: any,
  options?: { toast?: boolean }
) => {
  const shouldToast = options?.toast === true;

  try {
    const res = await fetch(`${BASE_URL}${url}`, {
      method,
      headers: {
        "Content-Type": "application/json",
      },
      cache: "no-store",
      ...(data && { body: JSON.stringify(data) }),
    });
    console.log(
      `========================= ${BASE_URL}${url}==================`
    );
    if (!res.ok) {
      if (shouldToast) {
        showToast({
          title: "API Error",
          message: `Request failed with status ${res.status}`,
          type: "error",
        });
      }
      throw new Error(`HTTP error! ${res.status}`);
    }

    return await res.json();
  } catch (err) {
    if (shouldToast) {
      showToast({
        title: "Network Error",
        message: "Unable to connect to the server.",
        type: "error",
      });
    }
    throw err;
  }
};

export const httpService = {
  get: (url: string, options?: { toast?: boolean }) =>
    request("GET", url, undefined, options),
  post: (url: string, data: any, options?: { toast?: boolean }) =>
    request("POST", url, data, options),
};
