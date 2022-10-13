using System.Numerics;
using UnityEngine;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;


public partial class TotemSmartContractDeployment : TotemSmartContractDeploymentBase
{
    public TotemSmartContractDeployment() : base(BYTECODE) { }
    public TotemSmartContractDeployment(string byteCode) : base(byteCode) { }
}

public class TotemSmartContractDeploymentBase : ContractDeploymentMessage
{
    public static string BYTECODE = "0x";
    public TotemSmartContractDeploymentBase() : base(BYTECODE) { }
    public TotemSmartContractDeploymentBase(string byteCode) : base(byteCode) { }

}

public partial class ApproveFunction : ApproveFunctionBase { }

[Function("approve")]
public class ApproveFunctionBase : FunctionMessage
{
    [Parameter("address", "to", 1)]
    public virtual string To { get; set; }
    [Parameter("uint256", "tokenId", 2)]
    public virtual BigInteger TokenId { get; set; }
}

public partial class BalanceOfFunction : BalanceOfFunctionBase { }

[Function("balanceOf", "uint256")]
public class BalanceOfFunctionBase : FunctionMessage
{
    [Parameter("address", "owner", 1)]
    public virtual string Owner { get; set; }
}

public partial class GetApprovedFunction : GetApprovedFunctionBase { }

[Function("getApproved", "address")]
public class GetApprovedFunctionBase : FunctionMessage
{
    [Parameter("uint256", "tokenId", 1)]
    public virtual BigInteger TokenId { get; set; }
}

public partial class IsApprovedForAllFunction : IsApprovedForAllFunctionBase { }

[Function("isApprovedForAll", "bool")]
public class IsApprovedForAllFunctionBase : FunctionMessage
{
    [Parameter("address", "owner", 1)]
    public virtual string Owner { get; set; }
    [Parameter("address", "operator", 2)]
    public virtual string Operator { get; set; }
}

public partial class NameFunction : NameFunctionBase { }

[Function("name", "string")]
public class NameFunctionBase : FunctionMessage
{

}

public partial class OwnerFunction : OwnerFunctionBase { }

[Function("owner", "address")]
public class OwnerFunctionBase : FunctionMessage
{

}

public partial class OwnerOfFunction : OwnerOfFunctionBase { }

[Function("ownerOf", "address")]
public class OwnerOfFunctionBase : FunctionMessage
{
    [Parameter("uint256", "tokenId", 1)]
    public virtual BigInteger TokenId { get; set; }
}

public partial class RenounceOwnershipFunction : RenounceOwnershipFunctionBase { }

[Function("renounceOwnership")]
public class RenounceOwnershipFunctionBase : FunctionMessage
{

}

public partial class SafeMintFunction : SafeMintFunctionBase { }

[Function("safeMint")]
public class SafeMintFunctionBase : FunctionMessage
{
    [Parameter("address", "to", 1)]
    public virtual string To { get; set; }
    [Parameter("string", "uri", 2)]
    public virtual string Uri { get; set; }
}

public partial class SafeTransferFromFunction : SafeTransferFromFunctionBase { }

[Function("safeTransferFrom")]
public class SafeTransferFromFunctionBase : FunctionMessage
{
    [Parameter("address", "from", 1)]
    public virtual string From { get; set; }
    [Parameter("address", "to", 2)]
    public virtual string To { get; set; }
    [Parameter("uint256", "tokenId", 3)]
    public virtual BigInteger TokenId { get; set; }
}

public partial class SafeTransferFrom1Function : SafeTransferFrom1FunctionBase { }

[Function("safeTransferFrom")]
public class SafeTransferFrom1FunctionBase : FunctionMessage
{
    [Parameter("address", "from", 1)]
    public virtual string From { get; set; }
    [Parameter("address", "to", 2)]
    public virtual string To { get; set; }
    [Parameter("uint256", "tokenId", 3)]
    public virtual BigInteger TokenId { get; set; }
    [Parameter("bytes", "data", 4)]
    public virtual byte[] Data { get; set; }
}

public partial class SetApprovalForAllFunction : SetApprovalForAllFunctionBase { }

[Function("setApprovalForAll")]
public class SetApprovalForAllFunctionBase : FunctionMessage
{
    [Parameter("address", "operator", 1)]
    public virtual string Operator { get; set; }
    [Parameter("bool", "approved", 2)]
    public virtual bool Approved { get; set; }
}

public partial class SupportsInterfaceFunction : SupportsInterfaceFunctionBase { }

[Function("supportsInterface", "bool")]
public class SupportsInterfaceFunctionBase : FunctionMessage
{
    [Parameter("bytes4", "interfaceId", 1)]
    public virtual byte[] InterfaceId { get; set; }
}

public partial class SymbolFunction : SymbolFunctionBase { }

[Function("symbol", "string")]
public class SymbolFunctionBase : FunctionMessage
{

}

public partial class TokenByIndexFunction : TokenByIndexFunctionBase { }

[Function("tokenByIndex", "uint256")]
public class TokenByIndexFunctionBase : FunctionMessage
{
    [Parameter("uint256", "index", 1)]
    public virtual BigInteger Index { get; set; }
}

public partial class TokenOfOwnerByIndexFunction : TokenOfOwnerByIndexFunctionBase { }

[Function("tokenOfOwnerByIndex", "uint256")]
public class TokenOfOwnerByIndexFunctionBase : FunctionMessage
{
    [Parameter("address", "owner", 1)]
    public virtual string Owner { get; set; }
    [Parameter("uint256", "index", 2)]
    public virtual BigInteger Index { get; set; }
}

public partial class TokenURIFunction : TokenURIFunctionBase { }

[Function("tokenURI", "string")]
public class TokenURIFunctionBase : FunctionMessage
{
    [Parameter("uint256", "tokenId", 1)]
    public virtual BigInteger TokenId { get; set; }
}

public partial class TotalSupplyFunction : TotalSupplyFunctionBase { }

[Function("totalSupply", "uint256")]
public class TotalSupplyFunctionBase : FunctionMessage
{

}

public partial class TransferFromFunction : TransferFromFunctionBase { }

[Function("transferFrom")]
public class TransferFromFunctionBase : FunctionMessage
{
    [Parameter("address", "from", 1)]
    public virtual string From { get; set; }
    [Parameter("address", "to", 2)]
    public virtual string To { get; set; }
    [Parameter("uint256", "tokenId", 3)]
    public virtual BigInteger TokenId { get; set; }
}

public partial class TransferOwnershipFunction : TransferOwnershipFunctionBase { }

[Function("transferOwnership")]
public class TransferOwnershipFunctionBase : FunctionMessage
{
    [Parameter("address", "newOwner", 1)]
    public virtual string NewOwner { get; set; }
}

public partial class ApprovalEventDTO : ApprovalEventDTOBase { }

[Event("Approval")]
public class ApprovalEventDTOBase : IEventDTO
{
    [Parameter("address", "owner", 1, true)]
    public virtual string Owner { get; set; }
    [Parameter("address", "approved", 2, true)]
    public virtual string Approved { get; set; }
    [Parameter("uint256", "tokenId", 3, true)]
    public virtual BigInteger TokenId { get; set; }
}

public partial class ApprovalForAllEventDTO : ApprovalForAllEventDTOBase { }

[Event("ApprovalForAll")]
public class ApprovalForAllEventDTOBase : IEventDTO
{
    [Parameter("address", "owner", 1, true)]
    public virtual string Owner { get; set; }
    [Parameter("address", "operator", 2, true)]
    public virtual string Operator { get; set; }
    [Parameter("bool", "approved", 3, false)]
    public virtual bool Approved { get; set; }
}

public partial class OwnershipTransferredEventDTO : OwnershipTransferredEventDTOBase { }

[Event("OwnershipTransferred")]
public class OwnershipTransferredEventDTOBase : IEventDTO
{
    [Parameter("address", "previousOwner", 1, true)]
    public virtual string PreviousOwner { get; set; }
    [Parameter("address", "newOwner", 2, true)]
    public virtual string NewOwner { get; set; }
}

public partial class TransferEventDTO : TransferEventDTOBase { }

[Event("Transfer")]
public class TransferEventDTOBase : IEventDTO
{
    [Parameter("address", "from", 1, true)]
    public virtual string From { get; set; }
    [Parameter("address", "to", 2, true)]
    public virtual string To { get; set; }
    [Parameter("uint256", "tokenId", 3, true)]
    public virtual BigInteger TokenId { get; set; }
}



public partial class BalanceOfOutputDTO : BalanceOfOutputDTOBase { }

[FunctionOutput]
public class BalanceOfOutputDTOBase : IFunctionOutputDTO
{
    [Parameter("uint256", "", 1)]
    public virtual BigInteger ReturnValue1 { get; set; }
}

public partial class GetApprovedOutputDTO : GetApprovedOutputDTOBase { }

[FunctionOutput]
public class GetApprovedOutputDTOBase : IFunctionOutputDTO
{
    [Parameter("address", "", 1)]
    public virtual string ReturnValue1 { get; set; }
}

public partial class IsApprovedForAllOutputDTO : IsApprovedForAllOutputDTOBase { }

[FunctionOutput]
public class IsApprovedForAllOutputDTOBase : IFunctionOutputDTO
{
    [Parameter("bool", "", 1)]
    public virtual bool ReturnValue1 { get; set; }
}

public partial class NameOutputDTO : NameOutputDTOBase { }

[FunctionOutput]
public class NameOutputDTOBase : IFunctionOutputDTO
{
    [Parameter("string", "", 1)]
    public virtual string ReturnValue1 { get; set; }
}

public partial class OwnerOutputDTO : OwnerOutputDTOBase { }

[FunctionOutput]
public class OwnerOutputDTOBase : IFunctionOutputDTO
{
    [Parameter("address", "", 1)]
    public virtual string ReturnValue1 { get; set; }
}

public partial class OwnerOfOutputDTO : OwnerOfOutputDTOBase { }

[FunctionOutput]
public class OwnerOfOutputDTOBase : IFunctionOutputDTO
{
    [Parameter("address", "", 1)]
    public virtual string ReturnValue1 { get; set; }
}











public partial class SupportsInterfaceOutputDTO : SupportsInterfaceOutputDTOBase { }

[FunctionOutput]
public class SupportsInterfaceOutputDTOBase : IFunctionOutputDTO
{
    [Parameter("bool", "", 1)]
    public virtual bool ReturnValue1 { get; set; }
}

public partial class SymbolOutputDTO : SymbolOutputDTOBase { }

[FunctionOutput]
public class SymbolOutputDTOBase : IFunctionOutputDTO
{
    [Parameter("string", "", 1)]
    public virtual string ReturnValue1 { get; set; }
}

public partial class TokenByIndexOutputDTO : TokenByIndexOutputDTOBase { }

[FunctionOutput]
public class TokenByIndexOutputDTOBase : IFunctionOutputDTO
{
    [Parameter("uint256", "", 1)]
    public virtual BigInteger ReturnValue1 { get; set; }
}

public partial class TokenOfOwnerByIndexOutputDTO : TokenOfOwnerByIndexOutputDTOBase { }

[FunctionOutput]
public class TokenOfOwnerByIndexOutputDTOBase : IFunctionOutputDTO
{
    [Parameter("uint256", "", 1)]
    public virtual BigInteger ReturnValue1 { get; set; }
}

public partial class TokenURIOutputDTO : TokenURIOutputDTOBase { }

[FunctionOutput]
public class TokenURIOutputDTOBase : IFunctionOutputDTO
{
    [Parameter("string", "", 1)]
    public virtual string ReturnValue1 { get; set; }
}

public partial class TotalSupplyOutputDTO : TotalSupplyOutputDTOBase { }

[FunctionOutput]
public class TotalSupplyOutputDTOBase : IFunctionOutputDTO
{
    [Parameter("uint256", "", 1)]
    public virtual BigInteger ReturnValue1 { get; set; }
}


