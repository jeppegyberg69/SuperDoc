// find en måde hvor jeg ikke skal bruge use client i layout fil. Dette er på nuværrende tidspunkt nødvendigt pga. at jeg kalder en hook metode i main function
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

export default function DetailsLayout({ params, children }: { params: { id: any }, children: any }) {
  const router = useRouter();
  const { data, error } = useGetCaseDetails(params.id);
  const session = getWebSession();

  return (
    <div className='page-layout h-full'>
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
            <span className="pl-2 self-center font-semibold text-xl">{data?.case?.title}</span>
          </div>
        )}
      />
      {session.user?.role !== Roles.User && <DetailsBanner details={data} />}
      <div className='page-layout-sidebar p-4'><DetailsSidebar caseId={params.id} /></div>
      <div className="page-layout-content">
        {children}
      </div>
    </div>
  );
}
