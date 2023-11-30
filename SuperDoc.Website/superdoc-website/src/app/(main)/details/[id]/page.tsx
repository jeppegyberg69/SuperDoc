"use client";

import { useGetCaseDetails } from "@/services/case-service";
import { Documents } from "./documents/documents"

export default function Details({ params }: { params: { id: any } }) {
	const { data, error } = useGetCaseDetails(params.id);

	if (!data) return <>loading...</>
	return (
		<Documents details={data}></Documents>
	)
}

