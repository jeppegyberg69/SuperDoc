"use client"

import { PageHeader } from "@/common/page-layout/page-header";

import { DetailsBanner } from "../details-banner/details-banner";
import { useGetCaseDetails } from "@/services/case-service";
import { DetailsSidebar } from "../details-sidebar/details-sidebar";
import { getWebSession } from "@/common/session-context/session-context";
import { Roles } from "@/common/access-control/access-control";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faAngleLeft } from "@fortawesome/free-solid-svg-icons/faAngleLeft";
import { Button } from "@/components/ui/button";
import { useRouter } from "next/navigation";
import { CaseDetails } from "@/models/case-details";
import { toast } from "@/components/ui/use-toast";

export default function DetailsLayout({ params, children }: { params: { id: any }, children: any }) {
  const router = useRouter();
  const { data, error } = useGetCaseDetails(params.id);
  const session = getWebSession();

  if (error) {
    toast({
      title: "Fejl",
      description: "Kunne ikke hente sagsdetaljer."
    })
  }

  return (
    <div className='page-layout h-full'>
      <DetailsHeader
        details={data}
        router={router}
      />
      {session.user?.role !== Roles.User && <DetailsBanner details={data} />}
      <div className='page-layout-sidebar p-4'><DetailsSidebar caseId={params.id} /></div>
      <div className="page-layout-content">
        {children}
      </div>
    </div>
  );
}

function DetailsHeader({ details, router }: { details: CaseDetails, router: ReturnType<typeof useRouter> }) {
  return (
    <PageHeader
      left={(
        <div className="flex flex-1">
          <Button
            className="hover:text-white"
            variant="ghost"
            onClick={() => { router.push("/") }}
          >
            <FontAwesomeIcon icon={faAngleLeft} />
          </Button>
          <span className="pl-2 self-center font-semibold text-xl">{details?.case?.title}</span>
        </div>
      )}
    />
  )
}
