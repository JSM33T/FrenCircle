/* eslint-disable @typescript-eslint/no-explicit-any */
const BASE_URL = process.env.PUBLIC_API_BASE_URL;

const request = async (method: "GET" | "POST", url: string, data?: any) => {
  const res = await fetch(`http://localhost:5035/api/changelog/grouped`, {
    method,
    headers: {
      "Content-Type": "application/json",
    },
    cache: "no-store",
    ...(data && { body: JSON.stringify(data) }),
  });
  console.log(`${BASE_URL}${url}`);
  if (!res.ok) throw new Error(`HTTP error! ${res.status}`);
  return res.json();
};

export const httpService = {
  get: (url: string) => request("GET", url),
  post: (url: string, data: any) => request("POST", url, data),
};
