export interface APIResponse<T> {
	status: number;
	message: string;
	data: T;
	hints: string[];
}