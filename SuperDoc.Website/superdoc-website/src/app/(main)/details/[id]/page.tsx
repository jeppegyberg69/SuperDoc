"use client";

import { useGetCaseDetails } from "@/services/case-service";
import { Documents } from "./documents/documents"
import { toast } from "@/components/ui/use-toast";

export default function Details({ params }: { params: { id: any } }) {
	const { data, error, isPending } = useGetCaseDetails(params.id);

	if (isPending)
		return <>loading...</>

	if (error) {
		toast({
			title: "Fejl",
			description: "Kunne ikke hente sagsdetaljer."
		})
	}

	return (
		<Documents details={data}></Documents>
	)
}

